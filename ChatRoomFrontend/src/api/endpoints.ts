import createClient, { Middleware } from "openapi-fetch"
import { paths } from './schema';
import { UserResponse, AuthenticationRequest, Space, ChatPeriod } from "./types";

const logRequestResponse: Middleware = {
    async onRequest({ request, schemaPath }) {
        console.log(request);
        console.log(schemaPath);
    },
    async onResponse({ response }) {
        console.log(response);
    }
};

const client = createClient<paths>({ baseUrl: 'http://localhost:5055' });
client.use(logRequestResponse);


export async function createUser(request: AuthenticationRequest) {
    const { data, response } = await client.POST("/api/Authentication/create-user", { body: { ...request } });
    if (response.ok) return data as UserResponse;
    throw new Error("Username already exists");
}

export async function authenticateUser(request: AuthenticationRequest) {
    const { data, response } = await client.POST("/api/Authentication/authorize-user", { body: { ...request } });
    if (response.ok) return data as UserResponse;
    throw new Error("Username or password is invalid");
}

export async function getSpacesByUserId(id: string | undefined) {
    if (!id) throw Error("An id must be provided")
    const { data } = await client.GET("/api/ChatSpaces/by-user/{id}", {
        params: {
            path: { id }
        }
    });
    return data as Space[];
}

export async function getLastMessagesInSpace(spaceId: string, getBeforeDate: Date, numberOfMessages: number) {
    const { data } = await client.GET("/api/ChatMessages", {
        params: {
            query: {
                spaceId,
                date: getBeforeDate.toISOString(),
                numberOfMessages
            }
        }
    });
    return data as unknown as ChatPeriod;
}