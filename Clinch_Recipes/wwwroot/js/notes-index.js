const moreNotesUrl = '/Notes/GetMoreNotes';
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

