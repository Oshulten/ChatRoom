import { InteractiveDataCellSupportedTypes } from "../components/interactiveDataCell";
import { GenericIdEntity } from "./genericIdEntity";

export class ChatMessageClass implements GenericIdEntity {
    id: string;
    userId: string;
    postedAt: Date;
    content: string;
    chatSpaceId: string;
    [key: string]: InteractiveDataCellSupportedTypes;

    static fromProperties(id: string, userId: string, postedAt: Date, content: string, chatSpaceId: string) {
        return new ChatMessageClass({ id, userId, postedAt, content, chatSpaceId });
    }

    fromObject(object: object) {
        return new ChatMessageClass(object);
    }

    constructor(fromObject: object) {
        const errorMessage = ["property '", "' is missing in deserialized object"];

        if ("id" in fromObject) this.id = fromObject.id as string;
        else throw new Error(`${errorMessage[0]}id${errorMessage[1]}`);

        if ("userId" in fromObject) this.userId = fromObject.userId as string;
        else throw new Error(`${errorMessage[0]}userId${errorMessage[1]}`);

        if ("postedAt" in fromObject) this.postedAt = new Date(fromObject.postedAt as string);
        else throw new Error(`${errorMessage[0]}postedAt${errorMessage[1]}`);

        if ("content" in fromObject) this.content = fromObject.content as string;
        else throw new Error(`${errorMessage[0]}content${errorMessage[1]}`);

        if ("chatSpaceId" in fromObject) this.chatSpaceId = fromObject.chatSpaceId as string;
        else throw new Error(`${errorMessage[0]}chatSpaceId${errorMessage[1]}`);
    }
}