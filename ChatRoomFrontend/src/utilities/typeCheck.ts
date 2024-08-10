export function typeCheck(subject: unknown) {
    if (subject === null) {
        return "null";
    }
    if (typeof (subject) == "object") {
        return subject.constructor.name;
    }
    return typeof (subject);
}