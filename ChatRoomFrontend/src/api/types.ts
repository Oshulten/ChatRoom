import { components } from '../api/schema';

export type Message = Required<components["schemas"]["ChatMessagePost"]>;
export type SpacePatch = components["schemas"]["ChatSpacePatch"];
export type Space = Required<components["schemas"]["ChatSpace"]>;
export type UserResponse = Required<components["schemas"]["ChatUserResponse"]>;
export type AuthenticationRequest = components["schemas"]["LoginRequest"];
export type ChatPeriod = {
    fromDate: string,
    toDate: string,
    earliest: number,
    messages: Message[]
}