import { PrimitiveType } from "../utilities/casting"

export interface GenericIdEntity {
  id: string
  [key: string]: PrimitiveType
}