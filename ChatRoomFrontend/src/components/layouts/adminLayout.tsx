import { useRef, useState } from "react";
import ObjectInspectorLayout from "./objectInspectorLayout";
import DataTablesLayout from "./dataTablesLayout";

/* eslint-disable react/react-in-jsx-scope */
type Tabs = "dataTables" | "objectInspector" | "chatRoom";

export default function AdminLayout() {
  const [activeTab, setActiveTab] = useState<Tabs>("chatRoom");
  const tabDataTables = useRef<HTMLAnchorElement | null>(null);
  const tabObjectInspector = useRef<HTMLAnchorElement | null>(null);

  const handleClick = (newActiveTab: Tabs) => {
    if (activeTab != newActiveTab) {
      setActiveTab(newActiveTab);
      switch (newActiveTab) {
        case "dataTables":
          tabDataTables.current!.classList.add("tab-active");
          tabObjectInspector.current!.classList.remove("tab-active");
          break;
        case "objectInspector":
          tabObjectInspector.current!.classList.add("tab-active");
          tabDataTables.current!.classList.remove("tab-active");
          break;
      }
    }
  }

  return (
    <div className="flex-row">
      <div role="tablist" className="tabs tabs-boxed sticky -top-0 z-10">
        <a ref={tabDataTables} role="tab" onClick={() => handleClick("dataTables")} className="tab tab-active">Live Database Table</a>
        <a ref={tabObjectInspector} role="tab" onClick={() => handleClick("objectInspector")} className="tab">Object Inspector</a>
      </div>
      <br />
      <br />
      <div className="-z-20">
        {activeTab == "objectInspector" &&
          <ObjectInspectorLayout />}
        {activeTab == "dataTables" &&
          <DataTablesLayout />}
      </div>
    </div>
  );
}
