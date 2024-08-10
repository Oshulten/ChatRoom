import React from 'react'
import { typeCheck } from '../utilities/typeCheck'
import { Primitive } from '../types/primitiveTypes'

function PrimitiveValue({ primitiveKey, primitive }: { primitiveKey?: string, primitive: Primitive }) {
    return <tr>
        <td>{primitiveKey ?? ""}</td>
        <td>{typeCheck(primitive)}</td>
        <td>{String(primitive)}</td>
    </tr>
}

function EmptyObject({ subjectKey, subject }: { subjectKey?: string, subject: object }) {
    return (
        <tr>
            <td>{subjectKey ?? ""}</td>
            <td>{typeCheck(subject)}</td>
            <td>{String(subject)}</td>
        </tr>
    )
}

interface ObjectInspectorProps {
    subject: object,
    subjectKey: string
}

export default function ObjectInspector({ subject, subjectKey }: ObjectInspectorProps) {
    const elements = Object.entries(subject).map(([key, value]) => {
        if (value instanceof Object) {
            if (Object.entries(value).length == 0) {
                return <EmptyObject subject={value} subjectKey={key} key={key} />
            }
            return <tr key={key}>
                <td colSpan={3}>
                    <ObjectInspector key={key} subjectKey={key} subject={value} />
                </td>
            </tr>
        }
        return <PrimitiveValue primitive={value} primitiveKey={key} key={key} />
    });

    return <>
        <div className="collapse collapse-arrow bg-base-200">
            <input type="checkbox" />
            <div className="text-left collapse-title text-m font-medium">{`${subjectKey}: [${typeCheck(subject)}]`}</div>
            <div className="collapse-content">
                <div className="overflow-x-auto">
                    <table className="table table-zebra">
                        <thead>
                            <tr>
                                <th>Identifier</th>
                                <th>Type</th>
                                <th>Value</th>
                            </tr>
                        </thead>
                        <tbody>
                            {elements}
                        </tbody>
                    </table>
                </div>
            </div>
        </div ></>
}