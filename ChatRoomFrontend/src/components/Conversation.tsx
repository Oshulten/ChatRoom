/* eslint-disable react/react-in-jsx-scope */
import { useContext } from "react";
import { AppContext } from "../main";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getLastMessagesInSpace } from "../api/endpoints";
import { Navigate } from "@tanstack/react-router";


export default function Conversation() {
    const { currentUser, currentSpace } = useContext(AppContext);

    const { data, isPending, isError, error } = useQuery({
        queryKey: ["messageSequence"],
        queryFn: () => getLastMessagesInSpace(currentSpace!.id, new Date(), 5),
        enabled: (currentSpace != undefined && currentUser != undefined),
        placeholderData: keepPreviousData,
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
        const sections = data.messages.map(message => {
            const user = data.users.find(user => user.id == message.userId);

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
                    {user.alias}{'\tposted at'}
                    <time className="text-xs opacity-50">{message.postedAt}</time>
                </div>
                <div className="chat-bubble">{message.content}</div>
            </div>
        });
        return sections;
    }
}