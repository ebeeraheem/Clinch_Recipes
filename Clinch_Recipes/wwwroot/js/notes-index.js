﻿const moreNotesUrl = '/Notes/GetMoreNotes';
const pageSize = 30;
let pageNumber = 2;

async function loadMore() {
    // Hide the load more button and display the loading spinner
    document.querySelector('#load-more').style.display = 'none';
    document.querySelector('.load-more-spinner').style.display = 'flex';

    // Fetch the next page of notes
    const response = await fetch(`${moreNotesUrl}?pageNumber=${pageNumber}`);
    if (!response.ok) {
        console.error(`Failed to load notes. Page: ${pageNumber}`);
        document.querySelector('#load-more').style.display = 'inline-block';
        document.querySelector('.load-more-spinner').style.display = 'none';
        return;
    }

    const pagedResult = await response.json();
    
    if (pagedResult.items.length <= 0) {
        // Hide the loading spinner and display the end message
        document.querySelector('.load-more-spinner').style.display = 'none';
        document.querySelector('hr').style.display = 'block';
        return;
    }

    // Render the notes
    renderNotes(pagedResult.items);

    // Increment the page number
    pageNumber++;

    // Hide the load more spinner
    document.querySelector('.load-more-spinner').style.display = 'none';

    if (pagedResult.currentPage === pagedResult.totalPages) {
        document.querySelector('hr').style.display = 'block';
    } else {
        document.querySelector('#load-more').style.display = 'inline-block';
    }
}

function renderNotes(notes) {
    const container = document.getElementById('notes-container');
    notes.forEach(note => {
        const noteDiv = document.createElement('div');
        noteDiv.classList.add('col', 'note-item');
        noteDiv.setAttribute('data-title', note.title);

        noteDiv.innerHTML = `
                <a class="noteItem" href="/Notes/Details/${note.id}">
                    <h4>${note.title}</h4>
                    <h6 class="note-date">${new Date(note.lastUpdatedDate).toLocaleDateString()}</h6>
                </a>
            `;
        container.appendChild(noteDiv);
    });
}

// Load more notes on button click
document.getElementById('load-more').addEventListener('click', loadMore);

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

// Search button functionality
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.querySelector('.searchNotes input[type="search"]');
    const searchButton = document.querySelector('.searchNotes button');
    const notesContainer = document.getElementById('notes-container');
    const noteItems = Array.from(notesContainer.getElementsByClassName('note-item'));

    function filterNotes(query) {
        query = query.toLowerCase();
        noteItems.forEach(item => {
            const title = item.getAttribute('data-title').toLowerCase();
            if (title.toLowerCase().includes(query)) {
                item.style.display = 'grid';
            } else {
                item.style.display = 'none';
            }
        });
    }

    // Search when button is clicked
    searchButton.addEventListener('click', () => {
        filterNotes(searchInput.value);
    });

    // Search as user types (debounced)
    let timeout;
    searchInput.addEventListener('input', () => {
        clearTimeout(timeout);
        timeout = setTimeout(() => filterNotes(searchInput.value), 300);
    });
});