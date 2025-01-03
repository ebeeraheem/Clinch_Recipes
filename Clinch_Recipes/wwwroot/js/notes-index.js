const moreNotesUrl = '/Notes/GetNotes';
const pageSize = 40;
let pageNumber = 2;
let isLoading = false;
let isSearchMode = false;
let searchTimeout;
let hasMorePages;
let notesOnFirstPage = 0;

// Elements cache for better performance
const elements = {
    loadMoreBtn: document.querySelector('#load-more'),
    spinner: document.querySelector('.load-more-spinner'),
    container: document.querySelector('#notes-container'),
    endMarker: document.querySelector('hr'),
    searchInput: document.querySelector('.searchNotes input[type="search"]'),
    searchButton: document.querySelector('.searchNotes button')
};


// Actions to be performed after the DOM content has loaded
document.addEventListener('DOMContentLoaded', function () {
    // Format date before displaying
    document.querySelectorAll('.note-date').forEach(function (element) {
        const utcDate = element.getAttribute('data-utc-date');
        element.textContent = formatDate(utcDate);

        notesOnFirstPage++;
    });

    // Save the loaded notes to local storage
    localStorage.setItem('existingNotes', JSON.stringify(elements.container.innerHTML));
    localStorage.setItem('firstPageNotes', notesOnFirstPage);

    // Listen to load more notes event
    elements.loadMoreBtn.addEventListener('click', loadMore);

    // Event listener for search input with debounce
    elements.searchInput.addEventListener('input', (e) => {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            handleSearch(e.target.value.trim());
        }, 300);
    });

    // Event listener for search button
    elements.searchButton.addEventListener('click', () => {
        handleSearch(elements.searchInput.value.trim());
    });
});

// Save note to session storage when navigating to details page
document.querySelectorAll('.noteItem').forEach(note => {
    note.addEventListener('click', function () {
        // Construct the note object from the note item
        const noteData = {
            id: note.getAttribute('data-id'),
            title: note.querySelector('h4').textContent,
            lastUpdatedDate: note.querySelector('.note-date').textContent,
            createdDate: note.querySelector('.created-date').textContent,
            content: note.querySelector('p').textContent
        };

        // Save the note object to session storage
        sessionStorage.setItem(`note_${noteData.id}`, JSON.stringify(noteData));
    });
});

// Function to handle search
async function handleSearch(searchTerm) {
    isSearchMode = searchTerm.length > 0;

    if (isSearchMode) {
        // Clear existing notes and show loading UI
        elements.container.innerHTML = '';
        updateUIState('loading');

        try {
            const response = await fetch(`/Notes/Search?term=${encodeURIComponent(searchTerm)}`);
            if (!response.ok) throw new Error('Search failed');

            const searchResults = await response.json();

            // Handle no more results
            if (searchResults.length === 0) {
                showNoResultsMessage('No matching items found.');
                updateUIState('end');
                return;
            }

            // Render the new notes
            renderNotes(searchResults);
            updateUIState('end');
        } catch (error) {
            console.error('Search error:', error);
            // Show user-friendly error message
            showErrorMessage('Failed to load more notes. Please try again.');
            updateUIState('error');
        }
    } else {
        const existingNotes = localStorage.getItem('existingNotes');
        elements.container.innerHTML = JSON.parse(existingNotes);

        // Get displayed alert
        const alert = document.querySelector('.alert');
        alert.style.display = 'none';

        // Update UI based on whether there are more pages
        const notesCount = localStorage.getItem('firstPageNotes');
        updateUIState(notesCount >= pageSize ? 'more' : 'end');
    }
}

// Load more notes
async function loadMore() {
    // Prevent multiple simultaneous requests
    if (isLoading) return;

    try {
        isLoading = true;
        updateUIState('loading');

        // Fetch the next page of notes with timeout for better UX
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 10000);

        const response = await fetch(
            `${moreNotesUrl}?pageNumber=${pageNumber}`,
            { signal: controller.signal }
        );

        clearTimeout(timeoutId);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const pagedResult = await response.json();

        // Handle empty or invalid response
        if (!pagedResult || !Array.isArray(pagedResult.items)) {
            throw new Error('Invalid response format');
        }

        // Handle no more results
        if (pagedResult.items.length === 0) {
            updateUIState('end');
            return;
        }

        // Render the new notes
        renderNotes(pagedResult.items);

        // Update pagination state
        pageNumber++;

        // Update UI based on whether there are more pages
        const isLastPage = pagedResult.currentPage >= pagedResult.totalPages ||
            pagedResult.items.length < pageSize;
        updateUIState(isLastPage ? 'end' : 'more');

    } catch (error) {
        console.error('Error loading more notes:', error);
        // Show user-friendly error message
        showErrorMessage('Failed to load more notes. Please try again.');
        updateUIState('error');

    } finally {
        isLoading = false;
    }
}

// Function to display notes
function renderNotes(notes) {
    const sortedNotes = notes.sort((a, b) => new Date(b.lastUpdatedDate) - new Date(a.lastUpdatedDate));

    // Create document fragment for better performance
    const fragment = document.createDocumentFragment();

    sortedNotes.forEach(note => {
        const noteDiv = document.createElement('div');
        noteDiv.classList.add('col', 'note-item');
        noteDiv.setAttribute('data-title', note.title);

        // Safely encode HTML content to prevent XSS
        const safeTitle = escapeHtml(note.title);
        const formattedDate = formatDate(note.lastUpdatedDate);

        noteDiv.innerHTML = `
                <a class="noteItem" href="/Notes/Details/${note.id}">
                    <h4>${safeTitle}</h4>
                    <h6 class="note-date">${formattedDate}</h6>
                </a>
            `;
        fragment.appendChild(noteDiv);
    });

    elements.container.appendChild(fragment);
}

// Helper function to safely escape HTML
function escapeHtml(unsafe) {
    if (!unsafe) return '';
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;")
        .replace(/`/g, "&#96;");
}

// Helper function to update UI state
function updateUIState(state) {
    switch (state) {
        case 'loading':
            elements.loadMoreBtn.style.display = 'none';
            elements.spinner.style.display = 'flex';
            elements.endMarker.style.display = 'none';
            break;
        case 'more':
            elements.loadMoreBtn.style.display = 'inline-block';
            elements.spinner.style.display = 'none';
            elements.endMarker.style.display = 'none';
            break;
        case 'end':
            elements.loadMoreBtn.style.display = 'none';
            elements.spinner.style.display = 'none';
            elements.endMarker.style.display = 'block';
            break;
        case 'error':
            elements.loadMoreBtn.style.display = 'inline-block';
            elements.spinner.style.display = 'none';
            elements.endMarker.style.display = 'none';
            break;
    }
}

// Helper function to show error message
function showErrorMessage(message) {
    const errorDiv = document.createElement('div');
    errorDiv.classList.add('alert', 'alert-danger', 'mt-3');
    errorDiv.textContent = message;
    elements.container.insertAdjacentElement('afterend', errorDiv);

    // Remove error message after 5 seconds
    setTimeout(() => errorDiv.remove(), 5000);
}

// Helper function to show no items found
function showNoResultsMessage(message) {
    const messageDiv = document.createElement('div');
    messageDiv.classList.add('alert', 'alert-info', 'mt-3');
    messageDiv.textContent = message;
    elements.container.insertAdjacentElement('afterend', messageDiv);
}