import { InteractiveDataCellSupportedTypes } from "../components/interactiveDataCell"

export interface GenericIdEntity {
  id: string
  [key: string]: InteractiveDataCellSupportedTypes
}