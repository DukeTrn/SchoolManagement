import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface User {
	accoundId: string | null;
	role: string | null;
}
const initialState: User = {
	accoundId: localStorage.getItem("accoundId") || null,
	role: localStorage.getItem("role") || null,
};
export const userSlice = createSlice({
	name: "users",
	initialState,
	reducers: {
		setUserAccount: (state, action: PayloadAction<User>) => {
			state.accoundId = action.payload.accoundId;
			state.role = action.payload.role;
		},
	},
});
export const { setUserAccount } = userSlice.actions;

const userReducer = userSlice.reducer;
export default userReducer;
