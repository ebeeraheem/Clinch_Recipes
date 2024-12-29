function populateNoteFromSessionStorage () {
    const url = window.location.href;
    const noteId = url.substring(url.lastIndexOf('/') + 1);
    let isRedirected = new URLSearchParams(window.location.search).has('redirected');

    // Get note from session storage
    const noteData = sessionStorage.getItem(`note_${noteId}`);

    // If note exists, populate the note details
    if (noteData) {
        const note = JSON.parse(noteData);

        document.querySelector('h2').textContent = note.title;
        document.querySelector('.note-date').textContent = note.createdDate;
        document.querySelector('.noteContent').innerHTML = note.content;
        document.querySelector('.upsert').setAttribute('href', `/Notes/Upsert/${note.id}`);
        document.querySelector('.delete').addEventListener('click', () => confirmDelete(note.id));
        return;

    } else if (!isRedirected) {
        window.location.href = `/Notes/GetNoteFromServer/${noteId}?redirected=true`;
        document.title = document.querySelector('h2').textContent;
    }
}

document.addEventListener('DOMContentLoaded', populateNoteFromSessionStorage());

function confirmDelete(noteId) {
    Swal.fire({
        title: "Are you sure?",
        text: "The note will be permanently deleted!",
        icon: "warning",
        reverseButtons: true,
        showCancelButton: true,
        confirmButtonColor: "#f00",
        cancelButtonColor: "#6c757d",
        confirmButtonText: "Delete"
    }).then((result) => {
        if (result.isConfirmed) {
            deleteNote(noteId);
        }
    });
}

function deleteNote(noteId) {
    fetch(`/Notes/Delete/${noteId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    title: "Deleted!",
                    text: "Note deleted successfully.",
                    icon: "success",
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    // Redirect to the notes list
                    window.location.href = '/';
                });

                // In case the timer expires, still redirect
                setTimeout(() => {
                    window.location.href = '/';
                }, 2000);
            } else {
                Swal.fire({
                    title: "Error!",
                    text: "There was an error deleting the note.",
                    icon: "error"
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: "Error!",
                text: "There was an error deleting the note.",
                icon: "error"
            });
        });
}