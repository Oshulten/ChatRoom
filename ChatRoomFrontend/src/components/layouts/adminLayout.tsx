import { useState } from "react";
import ObjectInspectorLayout from "./objectInspectorLayout";
import DataTablesLayout from "./dataTablesLayout";
import { Login } from "../login";

/* eslint-disable react/react-in-jsx-scope */
type Tabs = "dataTables" | "objectInspector" | "chatRoom";

export default function AdminLayout() {
  const [activeTab, setActiveTab] = useState<Tabs>("chatRoom");

  return (
    <div className="flex-row w-full">
      <div role="tablist" className="tabs tabs-boxed sticky -top-0 z-10 w-full">
        <a
          role="tab"
          onClick={() => setActiveTab("dataTables")}
          className={`tab ${activeTab == "dataTables" && "tab-active"}`}
        >
          Live Database Table
        </a>
        <a
          role="tab"
          onClick={() => setActiveTab("objectInspector")}
          className={`tab ${activeTab == "objectInspector" && "tab-active"}`}
        >
          Object Inspector
        </a>
        <a
          role="tab"
          onClick={() => setActiveTab("chatRoom")}
          className={`tab ${activeTab == "chatRoom" && "tab-active"}`}
        >
          Chat Spaces
        </a>
      </div>
      <br />
      <br />
      <div className="-z-20">
        {activeTab == "objectInspector" &&
          <ObjectInspectorLayout />}
        {activeTab == "dataTables" &&
          <DataTablesLayout />}
        {activeTab == "chatRoom" &&
          <Login />}
      </div>
    </div>
  );
}
