import { components } from '../api/schema';

export type Message = components["schemas"]["ChatMessage"];
export type MessagePatch = components["schemas"]["ChatMessagePatch"];
export type SpacePatch = components["schemas"]["ChatSpacePatch"];
export type Space = components["schemas"]["ChatSpace"];
export type UserResponse = Required<components["schemas"]["ChatUserResponse"]>;
export type AuthenticationRequest = components["schemas"]["LoginRequest"];