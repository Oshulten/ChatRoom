import { GenericIdEntity } from "./genericIdEntity";

export default interface ChatSpace extends GenericIdEntity {
    id: string,
    alias: string,
    password: string,
    userIds: string,
}
