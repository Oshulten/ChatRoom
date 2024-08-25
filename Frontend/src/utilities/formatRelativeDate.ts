export const formatRelativeDate = (date: Date, currentDate: Date) => {
    const [yearDif, monthDif, weekDif, dayDif] = [
        currentDate.getFullYear() - date.getFullYear(),
        currentDate.getMonth() - date.getMonth(),
        getWeek(currentDate) - getWeek(date),
        currentDate.getDay() - date.getDay()
    ];

    if (yearDif != 0) return date.toISOString().slice(0, 10);
    if (monthDif != 0) return `${monthDif} month${monthDif > 1 ? 's' : ''} ago`;
    if (weekDif != 0) return `${weekDif} week${weekDif > 1 ? 's' : ''} ago`;
    if (dayDif != 0) return `${dayDif} day${dayDif > 1 ? 's' : ''} ago`;
    return `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`
}

function getWeek(date: Date) {
    //Source: https://weeknumber.com/how-to/javascript
    date.setHours(0, 0, 0, 0);
    date.setDate(date.getDate() + 3 - (date.getDay() + 6) % 7);
    const week1 = new Date(date.getFullYear(), 0, 4);
    return 1 + Math.round(((date.getTime() - week1.getTime()) / 86400000
        - 3 + (week1.getDay() + 6) % 7) / 7);
}