import { components } from '../api/schema';

export type Message = Required<components["schemas"]["DtoMessage"]>;
export type Space = Required<components["schemas"]["DtoSpace"]>;
export type User = Required<components["schemas"]["DtoUser"]>;
export type Authentication = Required<components["schemas"]["DtoAuthentication"]>;

export type MessageSequence = {
    fromDate: string,
    toDate: string,
    earliest: boolean,
    messages: Message[],
    users: User[]
}