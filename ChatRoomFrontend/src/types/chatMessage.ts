import { GenericIdEntity } from "./genericIdEntity";

export default interface ChatMessage extends GenericIdEntity {
    id: string,
    userId: string,
    postedAt: string,
    content: string,
    chatSpaceId: string,
}
