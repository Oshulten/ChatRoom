import { GenericIdEntity } from "./genericIdEntity";

export interface SampleEntity extends GenericIdEntity {
    numberValue: number,
    booleanValue: boolean,
    stringValue: string
}