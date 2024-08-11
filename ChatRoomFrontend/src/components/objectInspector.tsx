/* eslint-disable react/react-in-jsx-scope */
import { typeCheck } from '../utilities/typeCheck'
import { InteractiveDataCell, InteractiveDataCellSupportedTypes } from './dataTable2';

interface PrimitiveValueProps {
    primitiveKey?: string,
    primitive: InteractiveDataCellSupportedTypes,
    onChange?: (newValue: InteractiveDataCellSupportedTypes, key: string | undefined) => void;
}

function PrimitiveValue({ primitiveKey, primitive, onChange }: PrimitiveValueProps) {
    const handleChange = (newValue: InteractiveDataCellSupportedTypes) => {
        if (onChange) {
            console.log(`onChange in PrimitiveValue - '${primitiveKey}': ${primitive}`)
            onChange(newValue, primitiveKey);
        }
    }

    return <tr>
        <td>{primitiveKey ?? ""}</td>
        <td>{typeCheck(primitive)}</td>
        <td><InteractiveDataCell value={primitive} onChange={newValue => handleChange(newValue)} /></td>
    </tr>
}

interface ObjectInspectorProps {
    subject: object,
    subjectKey: string,
    onChange: (newObject: InteractiveDataCellSupportedTypes, key: string | undefined) => void;
}

export default function ObjectInspector({ subject, subjectKey, onChange }: ObjectInspectorProps) {

    const reassembleSubject = (newProperty: InteractiveDataCellSupportedTypes, propertyKey: string | undefined) => {
        const typeInfo = typeCheck(subject);
        if (subject instanceof Object) {
            console.log(`Reassembling subject into ${subject.constructor.name}`);
            console.log(`\tType: ${typeInfo}`);
            console.log(`\tNew Property: ${JSON.stringify(newProperty)}`);
            console.log(`\tProperty Key: ${propertyKey}`);

            type BlankSlate = {
                [key: string]: InteractiveDataCellSupportedTypes
            }

            const filledSlate = Object.entries({ ...subject } as BlankSlate).reduce((accumulator, [key, value]) => {
                if (propertyKey === key) {
                    accumulator[key] = newProperty;
                    return accumulator;
                }
                accumulator[key] = value;
                return accumulator;
            }, {} as BlankSlate);
            if ("fromObject" in subject) {
                // eslint-disable-next-line @typescript-eslint/no-unsafe-function-type
                return (subject.fromObject as Function)(filledSlate as object);
            }
            return filledSlate as object;
        }
    }

    const handlePrimitiveChange = (newValue: InteractiveDataCellSupportedTypes, key: string | undefined) => {
        if (!key) {
            console.log(`onChange in ObjectInspector from PrimitiveValue - key for value ${newValue} is undefined`);
            return;
        }
        if (onChange) {
            console.log(`onChange in ObjectInspector from PrimitiveValue - '${key}': ${newValue}`)
            const reassembledSubject = reassembleSubject(newValue, key);
            console.log(`Reassembled subject - '${JSON.stringify(reassembledSubject)}`)
            onChange(reassembledSubject, key);
            return;
        }
        console.log(`onChange in ObjectInspector from PrimitiveValue - parent onChange is missing`)
    }

    const elements = Object.entries(subject).map(([key, value]) => {
        if (value instanceof Object && !(value instanceof Date)) {
            return <tr key={key}>
                <td colSpan={3}>
                    <ObjectInspector key={key} subjectKey={key} subject={value} onChange={(newValue, key) => handlePrimitiveChange(newValue, key)} />
                </td>
            </tr>
        }
        return <PrimitiveValue primitive={value} primitiveKey={key} key={key} onChange={(newValue, key) => handlePrimitiveChange(newValue, key)} />
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