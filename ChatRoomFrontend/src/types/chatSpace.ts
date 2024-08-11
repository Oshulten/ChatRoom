import { InteractiveDataCellSupportedTypes } from "../components/dataTable2";
import { GenericIdEntity } from "./genericIdEntity";

export default interface ChatSpace extends GenericIdEntity {
    id: string,
    alias: string,
    password: string,
    userIds: string,
}

export class ChatSpaceClass implements GenericIdEntity {
    id: string;
    alias: string;
    userIds: string[];
    [key: string]: InteractiveDataCellSupportedTypes;

    static fromProperties(id: string, alias: string, userIds: string[]) {
        return new ChatSpaceClass({ id, alias, userIds });
    }

    fromObject(object: object) {
        return new ChatSpaceClass(object);
    }

    constructor(fromObject: object) {
        const errorMessage = ["property '", "' is missing in deserialized object"];

        if ("id" in fromObject) this.id = fromObject.id as string;
        else throw new Error(`${errorMessage[0]}id${errorMessage[1]}`);

        if ("alias" in fromObject) this.alias = fromObject.alias as string;
        else throw new Error(`${errorMessage[0]}alias${errorMessage[1]}`);

        if ("userIds" in fromObject) this.userIds = fromObject.userIds as string[];
        else throw new Error(`${errorMessage[0]}userIds${errorMessage[1]}`);
    }
}
