export type PrimitiveType = string | boolean | number;

export function castToTypeInfo(typeValue: PrimitiveType): string {
    const instanceWrapper = Object(typeValue);
    let instanceOfClass;
    if (instanceWrapper instanceof Object) {
        instanceOfClass = "Object";
    }
    if (instanceWrapper instanceof Array) {
        instanceOfClass = "Array";
    }
    if (instanceWrapper instanceof Boolean) {
        instanceOfClass = "Boolean";
    }
    if (instanceWrapper instanceof Number) {
        instanceOfClass = "Number";
    }
    if (instanceWrapper instanceof Date) {
        instanceOfClass = "Date";
    }
    return (`typeof(value) = ${typeof (typeValue)}, instanceof ${instanceOfClass}`);
}

export function castStringToPrimitive(valueStringRepresentation: string, typeValue: PrimitiveType): PrimitiveType {
    switch (typeof (typeValue)) {
        case "number": return Number(valueStringRepresentation).valueOf();
        case "string": return String(valueStringRepresentation).valueOf();
        case "boolean": return valueStringRepresentation == "true" ? true : false;
        default: return Object(valueStringRepresentation).valueOf();
    }
}

export function castStringToObject(string: string, typeValue: string) {
    switch (typeValue) {
        case "string": return string;
        case "number": return Number.parseFloat(string);
        case "boolean": return (string === "true");
        case "undefined": return undefined;
        case "null": return null;
        case "Date": return new Date(string);
        default:
            throw new Error(`Value '${string}' has unhandled type '${typeValue}'`);
    }
}