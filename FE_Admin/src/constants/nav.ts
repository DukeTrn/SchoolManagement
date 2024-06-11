import {
	ContactRound,
	School,
	Component,
	BookMarked,
	Users2,
	Home,
	Columns3,
	LibraryBig,
	AlignEndHorizontal,
} from "lucide-react";

export const sideBarNavs = [
	{
		title: "Trang chủ",
		icon: Home,
		path: "/",
		variant: "default",
	},
	{
		title: "Tài khoản",
		icon: Users2,
		path: "/account",
		variant: "ghost",
	},
	{
		title: "Học sinh",
		icon: ContactRound,
		path: "/student",
		variant: "ghost",
	},
	{
		title: "Tổ bộ môn",
		icon: Component,
		path: "/group",
		variant: "ghost",
	},
	{
		title: "Giáo viên",
		icon: ContactRound,
		path: "/teacher",
		variant: "ghost",
	},
	{
		title: "Học kỳ",
		icon: BookMarked,
		path: "/semester",
		variant: "ghost",
	},
	{
		title: "Lớp học",
		icon: School,
		path: "/classroom",
		variant: "ghost",
	},
	{
		title: "Phân công giảng dạy",
		icon: Columns3,
		path: "/schedule",
		variant: "ghost",
	},
	{
		title: "Học tập",
		icon: LibraryBig,
		path: "/study",
		variant: "ghost",
	},
	{
		title: "Hạnh kiểm",
		icon: AlignEndHorizontal,
		path: "/study",
		variant: "ghost",
	},
];
