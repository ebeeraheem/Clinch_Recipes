const moreNotesUrl = '/Notes/GetNotes';
const pageSize = 30;
let pageNumber = 2;
let isLoading = false;

// Elements cache for better performance
const elements = {
    loadMoreBtn: document.getElementById('load-more'),
    spinner: document.querySelector('.load-more-spinner'),
    container: document.getElementById('notes-container'),
    endMarker: document.querySelector('hr'),
    searchInput: document.querySelector('.searchNotes input[type="search"]')
};

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


    //// Hide the load more button and display the loading spinner
    //document.querySelector('#load-more').style.display = 'none';
    //document.querySelector('.load-more-spinner').style.display = 'flex';

    //// Fetch the next page of notes
    //const response = await fetch(`${moreNotesUrl}?pageNumber=${pageNumber}&searchTerm=${query}`);
    //if (!response.ok) {
    //    console.error(`Failed to load notes. Page: ${pageNumber}. Error: ${response.statusText}`);
    //    document.querySelector('#load-more').style.display = 'inline-block';
    //    document.querySelector('.load-more-spinner').style.display = 'none';
    //    return;
    //}

    //const pagedResult = await response.json();
    
    //if (pagedResult.items.length <= 0) {
    //    // Hide the loading spinner and display the end message
    //    document.querySelector('.load-more-spinner').style.display = 'none';
    //    document.querySelector('hr').style.display = 'block';
    //    return;
    //} else {
    //    // Hide the end message and display the load more button
    //    document.querySelector('hr').style.display = 'none';
    //    document.querySelector('#load-more').style.display = 'inline-block';
    //}

    //// Render the notes
    //renderNotes(pagedResult.items);

    //// Increment the page number
    //pageNumber++;

    //// Hide the load more spinner
    //document.querySelector('.load-more-spinner').style.display = 'none';

    //if (pagedResult.currentPage >= pagedResult.totalPages || pagedResult.items.length < pageSize) {
    //    document.querySelector('hr').style.display = 'block';
    //    document.querySelector('#load-more').style.display = 'none';
    //} else {
    //    document.querySelector('#load-more').style.display = 'inline-block';
    //}
}

function renderNotes(notes) {
    // Create document fragment for better performance
    const fragment = document.createDocumentFragment();

    notes.forEach(note => {
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
        .replace(/'/g, "&#039;");
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
    // You can implement your preferred error display method here
    // For example, showing a toast notification or an alert
    const errorDiv = document.createElement('div');
    errorDiv.classList.add('alert', 'alert-danger', 'mt-3');
    errorDiv.textContent = message;
    elements.container.insertAdjacentElement('afterend', errorDiv);

    // Remove error message after 5 seconds
    setTimeout(() => errorDiv.remove(), 5000);
}

// Event listener with debouncing
elements.loadMoreBtn.addEventListener('click', () => {
    loadMore(elements.searchInput.value);
});

//// Load more notes on button click
//document.getElementById('load-more').addEventListener('click', function () {
//    const searchInput = document.querySelector('.searchNotes input[type="search"]');
//    loadMore(searchInput.value);
//});

//// Create a function to clear notes from the page
//function clearNotes() {
//    const notesContainer = document.getElementById('notes-container');
//    while (notesContainer.firstChild) {
//        notesContainer.removeChild(notesContainer.firstChild);
//    }

//    // Reset the page number
//    pageNumber = 2;

//    // Hide the end message
//    document.querySelector('hr').style.display = 'none';
//}

// Save note to session storage on click
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

// Format date before displaying
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.note-date').forEach(function (element) {
        const utcDate = element.getAttribute('data-utc-date');
        element.textContent = formatDate(utcDate);
    });
});

//// Search notes functionality
//document.addEventListener('DOMContentLoaded', function () {
//    const searchInput = document.querySelector('.searchNotes input[type="search"]');
//    const searchButton = document.querySelector('.searchNotes button');
//    const notesContainer = document.querySelector('#notes-container');
//    const noteItems = Array.from(notesContainer.querySelectorAll('.note-item'));

//    function filterNotes(query) {
//        query = query.toLowerCase();
//        noteItems.forEach(item => {
//            const title = item.getAttribute('data-title').toLowerCase();
//            if (title.toLowerCase().includes(query)) {
//                item.style.display = 'grid';
//            } else {
//                item.style.display = 'none';
//            }
//        });
//    }

//    // Write function to search notes from the GetNotes endpoint
//    async function searchNotes(query) {
//        pageNumber = 1;
//        const response = await fetch(`${moreNotesUrl}?pageNumber=${pageNumber}&searchTerm=${query}`);

//        if (!response.ok) {
//            console.error(`Failed to load notes. Page: ${pageNumber}. Status: ${response.status}. Message: ${response.statusText}`);
//            return;
//        }

//        noteItems.forEach(item => {
//            item.style.display = 'none';
//        });

//        const pagedResult = await response.json();
//        renderNotes(pagedResult.items);

//        pageNumber++;
//    }

//    // Search when button is clicked
//    searchButton.addEventListener('click', () => {
//        searchNotes(searchInput.value);
//    });

//    // Search as user types (debounced)
//    let timeout;
//    searchInput.addEventListener('input', () => {
//        clearTimeout(timeout);
//        timeout = setTimeout(() => searchNotes(searchInput.value), 300);
//    });
//});