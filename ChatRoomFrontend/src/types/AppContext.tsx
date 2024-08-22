import { Space, User } from "../api/types"

export interface AppContext {
    currentUser?: User
    currentSpace?: Space
}

export const defaultAppContext = {
    currentUser: undefined,
    currentSpace: undefined
}