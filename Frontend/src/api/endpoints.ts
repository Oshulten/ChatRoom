import createClient, { Middleware } from "openapi-fetch"
import { paths } from './schema';
import { User, Authentication, Space, MessageSequence, MessagePost, Message } from "./types";

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


export async function createUser(request: Authentication) {
    const { data, response } = await client.POST("/api/Authentication/create-user", { body: { ...request } });

    if (response.ok) return data as User;

    throw new Error("Username already exists");
}

export async function authenticateUser(request: Authentication) {
    const { data, response } = await client.POST("/api/Authentication/authorize-user", { body: { ...request } });

    if (response.ok) return data as User;

    throw new Error("Username or password is invalid");
}

export async function getSpacesByUserId(userId: string | undefined) {
    if (!userId) throw Error("An id must be provided")

    const { data } = await client.GET("/api/Spaces", {
        params: {
            query: { userId }
        }
    });

    return data as Space[];
}

export async function getUserByUserId(userId: string) {
    const { data } = await client.GET("/api/Users/{userId}", {
        params: {
            path: { userId }
        }
    });

    return data as User;
}

export async function getLastMessagesInSpace(spaceId: string, getBeforeDate: Date, numberOfMessages: number) {
    const { data } = await client.GET("/api/Messages", {
        params: {
            query: {
                spaceId: spaceId,
                messagesBefore: getBeforeDate.toISOString(),
                numberOfMessages: numberOfMessages,
            }
        }
    });

    return data as unknown as MessageSequence;
}

export async function postMessage({ content, spaceId, userId }: MessagePost) {
    console.log({ content, spaceId, userId });
    const { data } = await client.POST("/api/Messages", {
        body: { content, spaceId, userId }
    });
    return data as Message;
}