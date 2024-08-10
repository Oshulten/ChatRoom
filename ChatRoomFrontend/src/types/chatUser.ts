import { GenericIdEntity } from "./genericIdEntity";

export default interface ChatUser extends GenericIdEntity {
    alias: string,
    password: string,
    joinedAt: string,
    admin: boolean
}

export class ChatUserClass {
    alias: string;
    password: string;
    joinedAt: Date;
    admin: boolean;

    static fromProperties(alias: string, password: string, joinedAt: Date, admin: boolean) {
        return new ChatUserClass({ alias, password, joinedAt, admin });
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