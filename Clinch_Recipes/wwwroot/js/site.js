


//// Convert date to local time format
//function formatDate(dateString) {
//    const date = new Date(dateString + 'Z');
//    const now = new Date();

//    const isToday = date.toDateString() === now.toDateString();
//    const isThisYear = date.getFullYear() === now.getFullYear();

//    const day = date.getDate();
//    const month = date.toLocaleString('default', { month: 'short' });
//    const year = date.getFullYear();
//    const hours = date.getHours() % 12 || 12;
//    const minutes = date.getMinutes().toString().padStart(2, '0');
//    const ampm = date.getHours() >= 12 ? 'PM' : 'AM';
//    const daySuffix = day % 10 === 1 && day !== 11 ? 'st' : day % 10 === 2 && day !== 12 ? 'nd' : day % 10 === 3 && day !== 13 ? 'rd' : 'th';

//    if (isToday) {
//        return `${hours}:${minutes} ${ampm}`;
//    }

//    let formattedDate = `${day}${daySuffix} ${month}`;
//    if (!isThisYear) {
//        formattedDate += ` ${year}`;
//    }

//    return `${formattedDate} at ${hours}:${minutes} ${ampm}`;
//}

function formatDate(dateString) {
    // Use Intl.DateTimeFormat for better internationalization
    const date = new Date(dateString + 'Z');
    const now = new Date();
    const isToday = date.toDateString() === now.toDateString();

    const formatter = new Intl.DateTimeFormat('en-US', {
        ...(isToday ? {
            hour: 'numeric',
            minute: '2-digit',
            hour12: true
        } : {
            day: 'numeric',
            month: 'short',
            year: date.getFullYear() === now.getFullYear() ? undefined : 'numeric',
            hour: 'numeric',
            minute: '2-digit',
            hour12: true
        })
    });

    return formatter.format(date).replace(',', ' at');
}
