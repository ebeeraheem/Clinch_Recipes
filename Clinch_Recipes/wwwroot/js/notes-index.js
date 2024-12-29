const fetchUrl = '@Url.Action("GetMoreNotes", "Notes")';
let page = 2;
let isLoading = false;
let hasMoreNotes = true;
const notesCache = {}; // Object to store cached pages

const pageSize = 30;
let pageNumber = 2;

async function loadMore() {
    const moreNotesUrl = '/Notes/GetMoreNotes';

    // Display the loading spinner
    document.getElementById('loading').style.display = 'block';

    // Fetch the next page of notes
    const response = await fetch(`${moreNotesUrl}?pageNumber=${pageNumber}`);
    if (!response.ok) {
        console.error(`Failed to load notes. Page: ${pageNumber}`);
        return;
    }

    const pagedResult = await response.json();
    
    if (pagedResult.Items.length <= 0) {
        // Hide the loading spinner and display the end message
        document.getElementById('loading').style.display = 'none';
        document.getElementById('end-message').style.display = 'block';
        return;
    }

    // Render the notes
    renderNotes(pagedResult.Items);

    // Increment the page number
    pageNumber++;

    // Hide the loading spinner
    document.getElementById('loading').style.display = 'none';

    // Check if there are more notes to load
    if (pagedResult.Items.length < pageSize) {
        document.getElementById('end-message').style.display = 'block';
    } else {
        // Display the load more button
        document.getElementById('load-more').style.display = 'block';
    }
}

async function loadMoreNotes() {
    if (isLoading || !hasMoreNotes) return;
    isLoading = true;

    document.getElementById('loading').style.display = 'block';

    try {
        // Check if the page is already cached
        if (notesCache[page]) {
            renderNotes(notesCache[page]);
            isLoading = false;
            document.getElementById('loading').style.display = 'none';
            page++;
            return;
        }

        const response = await fetch(`${fetchUrl}?page=${page}&pageSize=20`);
        if (!response.ok) throw new Error('Failed to load notes');

        const notes = await response.json();

        if (notes.length <= 0) {
            hasMoreNotes = false;
            document.getElementById('end-message').style.display = 'block';
            return;
        }

        notesCache[page] = notes; // Cache the current page

        renderNotes(notes);

        if (notes.length > 0) {
            page++; // Increment the page number
        }
    } catch (error) {
        console.error(error);
    } finally {
        document.getElementById('loading').style.display = 'none';
        isLoading = false;
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

// Infinite scrolling logic
window.addEventListener('scroll', () => {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 200 && !isLoading) {
        loadMoreNotes();
    }
});
