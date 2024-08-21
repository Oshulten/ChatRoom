/* eslint-disable react/react-in-jsx-scope */

import { useContext } from "react";
import { Message, User } from "../api/types";
import { GlobalContext } from "../main";
import ConversationBubble from "./ConversationBubble";

interface Props {
    messages: Message[]
    users: User[]
}

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

export default function Conversation({ messages, users }: Props) {
    const context = useContext(GlobalContext);

    if (messages.length == 0 || users.length == 0 || !context.currentUser) return <p>Lack of messages or users, or no current user</p>

    const chatSections: ChatSection[] = [];
    let currentSide: MessageSide | null = null;
    let currentSenderId: string | null = null;
    const lastSenderId: string | null = null;

    const userId = context.currentUser.id;

    messages.forEach((message: Message) => {
        const messageSide: MessageSide = message.userId === userId ? "user" : "sender";
        const currentUser = users.find((user: User) => user.id === message.userId)!;
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