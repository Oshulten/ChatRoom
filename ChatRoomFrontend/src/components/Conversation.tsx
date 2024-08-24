/* eslint-disable react/react-in-jsx-scope */
import { useContext, useRef } from "react";
import { AppContext } from "../main";
import { useInfiniteQuery } from "@tanstack/react-query";
import { getLastMessagesInSpace } from "../api/endpoints";
import { Navigate } from "@tanstack/react-router";
import { formatRelativeDate } from "../utilities/formatRelativeDate";
import { MessageSequence } from "../api/types";

export default function Conversation() {
    const { currentUser, currentSpace } = useContext(AppContext);
    const upperBoundary = useRef<HTMLDivElement>(null);
    const lowerBoundary = useRef<HTMLDivElement>(null);
    const messagesPerFetch = 10;

    const {
        data,
        error,
        isPending,
        isError,
        fetchNextPage,
        hasNextPage
    } = useInfiniteQuery({
        queryKey: ["messageSequence"],
        queryFn: ({ pageParam }) => {
            return getLastMessagesInSpace(currentSpace!.id, pageParam, messagesPerFetch);
        },
        initialPageParam: new Date(),
        getNextPageParam: (lastPage: MessageSequence) => lastPage.earliest ? null : new Date(lastPage.fromDate),
        enabled: (currentSpace != undefined && currentUser != undefined),
    });

    if (!currentSpace || !currentUser) {
        const errorMessage = `Context lacks currentSpace or currentUser: ${JSON.stringify({ currentUser, currentSpace })}`
        console.error(errorMessage);
        return <Navigate to="/login" />;
    }

    if (isPending) {
        return <p>Fetching message sequence...</p>;
    }

    if (isError) {
        return <p>{JSON.stringify(error)}</p>;
    }

    if (data) {
        console.log(data);
        const sections = data.pages.map(sequence => {
            return sequence.messages.map(message => {
                const user = sequence.users.find(user => user.id == message.userId);

                if (user == undefined) {
                    return;
                }

                return <div key={message.postedAt} className={`chat chat-${(user.id == currentUser.id) ? 'end' : 'start'}`}>
                    <div className="chat-image avatar">
                        <div className="w-10 rounded-full">
                            <img
                                alt="Tailwind CSS chat bubble component"
                                src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
                        </div>
                    </div>
                    <div className="chat-header">
                        {user.alias}
                        <time className="text-xs opacity-50">
                            {`\tposted ${formatRelativeDate(new Date(message.postedAt), new Date(Date.now()))}`}
                        </time>
                    </div>
                    <div className="chat-bubble">{message.content}</div>
                </div>
            })
        }).flat();

        return (
            <>
                <button onClick={() => {
                    if (hasNextPage) {
                        console.log(`Fetching next page`);
                        fetchNextPage();
                        return;
                    }
                    console.log("Out of pages");
                }}>Fetch more messages</button>

                <button onClick={() => {
                    console.log(lowerBoundary.current != undefined);
                    lowerBoundary.current!.scrollIntoView({ behavior: "smooth" });
                }}>Scroll down</button>
                <div className="h-96 overflow-auto p-8">
                    <div ref={upperBoundary}></div>
                    {sections.reverse()}
                    <div ref={lowerBoundary}></div>
                </div>
            </>
        );
    }
}