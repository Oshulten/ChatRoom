export function dateRepresentationRelativeToNow(date: Date, now: Date) {
    if (date.getDate() != now.getDate() || date.getMonth() != now.getMonth() || date.getFullYear() != now.getFullYear()) {
        return date.toDateString();
    }
    const difference = date.getTime() - now.getTime();
    const differenceSeconds = Math.floor(difference % 1000);
    const differenceMinutes = Math.floor(differenceSeconds % 60);
    const differenceHours = Math.floor(differenceMinutes % 60);
    if (Math.abs(differenceSeconds) < 60) {
        if (differenceSeconds > 0) return `${differenceSeconds} seconds ago`;
        return `${differenceSeconds} seconds from now`;
    }
    if (Math.abs(differenceMinutes) < 60) {
        if (differenceMinutes > 0) return `${differenceMinutes} minutes ago`;
        return `${differenceMinutes} minutes from now`;
    }
    if (Math.abs(differenceHours) < 24) {
        if (differenceHours > 0) return `${differenceHours} hours ago`;
        return `${differenceHours} hours from now`;
    }
    return "at some point";
}