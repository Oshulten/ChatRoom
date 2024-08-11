import { GenericIdEntity } from "./genericIdEntity";

export default interface ChatUser extends GenericIdEntity {
    alias: string,
    password: string,
    joinedAt: string,
    admin: boolean
}

interface StringValidationParameters {
    minLength?: number,
    maxLength?: number,
}

export class StringValidation {
    minLength?: number;
    maxLength?: number;

    constructor({ minLength, maxLength }: StringValidationParameters) {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

    validate(subject: string): string[] {
        const errorMessages: string[] = [];
        if (this.maxLength && subject.length > this.maxLength) {
            errorMessages.push(`Length cannot be longer than ${this.maxLength} characters`);
        }
        if (this.minLength && subject.length < this.minLength) {
            errorMessages.push(`Length must be longer than ${this.minLength - 1} characters`);
        }
        return errorMessages;
    }
}

export class ChatUserClass {
    alias: string;
    password: string;
    joinedAt: Date;
    admin: boolean;

    static fromProperties(alias: string, password: string, joinedAt: Date, admin: boolean) {
        return new ChatUserClass({ alias, password, joinedAt, admin });
    }

    static fromObject(object: object) {
        return new ChatUserClass(object);
    }

    fromObject(object: object) {
        return new ChatUserClass(object);
    }

    constructor(fromObject: object) {
        const errorMessage = ["property '", "' is missing in deserialized object"];
        if ("alias" in fromObject) this.alias = fromObject.alias as string;
        else throw new Error(`${errorMessage[0]}alias${errorMessage[1]}`);

        if ("password" in fromObject) this.password = fromObject.password as string;
        else throw new Error(`${errorMessage[0]}password${errorMessage[1]}`);

        if ("joinedAt" in fromObject) this.joinedAt = new Date(fromObject.joinedAt as string);
        else throw new Error(`${errorMessage[0]}joinedAt${errorMessage[1]}`);

        if ("admin" in fromObject) this.admin = fromObject.admin as boolean;
        else throw new Error(`${errorMessage[0]}admin${errorMessage[1]}`);
    }
}