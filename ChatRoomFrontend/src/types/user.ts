import { GenericIdEntity } from "./genericIdEntity";

export default interface User extends GenericIdEntity {
    alias: string,
    password: string,
}