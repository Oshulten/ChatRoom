import PrimitiveDataTable from "./dataTable";

const baseUrl = "http://localhost:5055/api";

/* eslint-disable react/react-in-jsx-scope */
export default function DataTablesLayout({ visible }: { visible: boolean }) {
    return (
        <div hidden={!visible}>
            <PrimitiveDataTable endpoint={`${baseUrl}/ChatMessages`} label="Chat Messages" showId={true} />
        </div>
    );
}