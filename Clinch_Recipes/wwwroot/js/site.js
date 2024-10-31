


// Convert date to local time format
function formatDate(dateString) {
    const date = new Date(dateString + 'Z');
    const now = new Date();

    const isToday = date.toDateString() === now.toDateString();
    const isThisYear = date.getFullYear() === now.getFullYear();

    const day = date.getDate();
    const month = date.toLocaleString('default', { month: 'short' });
    const year = date.getFullYear();
    const hours = date.getHours() % 12 || 12;
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const ampm = date.getHours() >= 12 ? 'PM' : 'AM';
    const daySuffix = day % 10 === 1 && day !== 11 ? 'st' : day % 10 === 2 && day !== 12 ? 'nd' : day % 10 === 3 && day !== 13 ? 'rd' : 'th';

    if (isToday) {
        return `${hours}:${minutes} ${ampm}`;
    }

    let formattedDate = `${day}${daySuffix} ${month}`;
    if (!isThisYear) {
        formattedDate += ` ${year}`;
    }

    return `${formattedDate} at ${hours}:${minutes} ${ampm}`;
}

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