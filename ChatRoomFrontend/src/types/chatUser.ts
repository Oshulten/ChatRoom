import { InteractiveDataCellSupportedTypes } from "../components/interactiveDataCell";
import { GenericIdEntity } from "./genericIdEntity";

export class ChatUserClass implements GenericIdEntity {
    id: string;
    alias: string;
    password: string;
    joinedAt: Date;
    admin: boolean;
    [key: string]: InteractiveDataCellSupportedTypes;

    static fromProperties(id: string, alias: string, password: string, joinedAt: Date, admin: boolean) {
        return new ChatUserClass({ id, alias, password, joinedAt, admin });
    }

    static fromObject(object: object) {
        return new ChatUserClass(object);
    }

    fromObject(object: object) {
        return new ChatUserClass(object);
    }

    constructor(fromObject: object) {
        const errorMessage = ["property '", "' is missing in deserialized object"];

        if ("id" in fromObject) this.id = fromObject.id as string;
        else throw new Error(`${errorMessage[0]}id${errorMessage[1]}`);

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