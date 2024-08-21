/* eslint-disable react/react-in-jsx-scope */
interface Props {
    messageContent: string
    userAlias: string
    dateSent: string
}

export default function ConversationBubble({ messageContent, userAlias, dateSent }: Props) {
    console.log(messageContent);
    console.log(userAlias);
    console.log(dateSent);
    return (<>
        <div className="chat-header">
            {userAlias}
            <br />
            <time className="text-xs opacity-50">{dateSent}</time>
        </div>
        <div className="chat-bubble">{messageContent}</div>
    </>);
}