/* eslint-disable react/react-in-jsx-scope */
import { useContext } from "react";
import { Message } from "../api/types";
import { AppContext } from "../main";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import ConversationBubble from "./ConversationBubble";
import { getLastMessagesInSpace, getUserByUserId } from "../api/endpoints";

type AugmentedMessage = {
    content: string,
    postedAt: string,
    userAlias: string;
};

type MessageSide = "user" | "sender";

type ChatSection = {
    messages: AugmentedMessage[];
    side: MessageSide;
};

export default function Conversation() {
    const { currentUser, currentSpace } = useContext(AppContext);

    const messagesQuery = useQuery({
        queryKey: ["messages", currentSpace!.id],
        queryFn: () => getLastMessagesInSpace(currentSpace!.id, new Date(), 5),
        enabled: (currentSpace != undefined && currentUser != undefined),
        placeholderData: keepPreviousData,
    });

    const usersQuery = useQuery({
        queryKey: ["usersInSpace"],
        queryFn: async () => {
            const distinctUserIds = [...new Set(messagesQuery.data?.messages.map(message => message.userId))];
            const userPromises = distinctUserIds.map(id => getUserByUserId(id));
            const users = await Promise.all(userPromises);
            return users;
        },
        enabled: messagesQuery.isSuccess
    })

    if (!currentSpace || !currentUser) {
        const errorMessage = `Context lacks currentSpace or currentUser: ${JSON.stringify(context)}`
        console.error(errorMessage);
        return <p>{errorMessage}</p>
    }

    if (!messagesQuery.data || !usersQuery.data) {
        <p>Loading...</p>
    }

    const sortedMessages = messagesQuery.data!.messages.sort((a, b) => {
        return (new Date(a.postedAt)) > (new Date(b.postedAt)) ? 1 : -1
    })

    const chatSections: ChatSection[] = [];
    let currentSide: MessageSide | null = null;
    let currentSenderId: string | null = null;
    const lastSenderId: string | null = null;

    sortedMessages.forEach((message: Message) => {
        const messageSide: MessageSide = message.userId === currentUser.id ? "user" : "sender";
        const userAlias = currentUser.alias;

        if (lastSenderId == currentUser.id) {
            chatSections.at(-1)!.messages.at(-1)!.content += message.content;
            return;
        }

        const augmentedMessage: AugmentedMessage = {
            userAlias: userAlias ? userAlias : "Anonymous",
            content: message.content,
            postedAt: message.postedAt,
        };

        if (!currentSenderId || currentSenderId !== message.userId) {
            currentSide = messageSide;
            currentSenderId = message.userId;
            chatSections.push({
                messages: [augmentedMessage],
                side: currentSide,
            });
            return;
        }
        chatSections.at(-1)!.messages.push(augmentedMessage);
    });

    return (
        <>
            <br></br>
            <h2>ChatDialog</h2>
            {chatSections.map((section: ChatSection, i) => {
                return (
                    <div
                        key={i}
                        className={section.side == "user" ? "chat chat-end" : "chat chat-start"}>
                        {section.messages.map((message: AugmentedMessage, index: number) => {
                            return <ConversationBubble
                                key={index}
                                messageContent={message.content}
                                userAlias={message.userAlias}
                                dateSent={message.postedAt} />;
                        })}
                    </div>
                );
            })}
        </>
    );
}