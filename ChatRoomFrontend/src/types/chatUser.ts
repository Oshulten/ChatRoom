import { GenericIdEntity } from "./genericIdEntity";

export default interface ChatUser extends GenericIdEntity {
    alias: string,
    password: string,
    joinedAt: string,
    admin: boolean
}