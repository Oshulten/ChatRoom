import { ChatMessageClass } from "../../types/chatMessage";
import { ChatSpaceClass } from "../../types/chatSpace";
import { ChatUserClass } from "../../types/chatUser";
import { InteractiveDataTable } from "../dataTable";

const baseUrl = "http://localhost:5055/api";

/* eslint-disable react/react-in-jsx-scope */
export default function DataTablesLayout() {
    return (
        <>
            <br />
            <InteractiveDataTable<ChatUserClass> endpoint={`${baseUrl}/ChatUsers`} label="Chat Users" />
            <br />
            <InteractiveDataTable<ChatSpaceClass> endpoint={`${baseUrl}/ChatSpaces`} label="Chat Spaces" />
            <br />
            <InteractiveDataTable<ChatMessageClass> endpoint={`${baseUrl}/ChatMessages`} label="Chat Messages" />
            <br />
        </>
    );
}