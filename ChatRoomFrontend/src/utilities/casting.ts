/* eslint-disable no-case-declarations */

export function typeCheck(subject: unknown) {
    if (subject === null) {
        return "null";
    }
    if (typeof (subject) == "object") {
        return subject.constructor.name;
    }
    return typeof (subject);
}

export function castStringToObject(string: string, typeValue: string) {
    switch (typeValue) {
        case "string": return string;
        case "number": return Number.parseFloat(string);
        case "boolean": return (string === "true");
        case "undefined": return undefined;
        case "null": return null;
        case "Date":
            const newDate = new Date(Date.parse(string));
            return newDate;
        default:
            throw new Error(`Value '${string}' has unhandled type '${typeValue}'`);
    }
}