// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function formatDate(dateString) {
    const date = new Date(dateString);
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