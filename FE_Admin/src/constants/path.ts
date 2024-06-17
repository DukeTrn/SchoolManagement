const path = {
	home: "/home",
	account: "/account",
	student: "/student",
	department: "/department",
	departmentDetails: "/department/:id",
	teacher: "/teacher",
	semester: "/semester",
	semesterDetail: "/semester/:id",
	classroom: "/classroom",
	classroomDetail: "/classroom/:id",
	assignment: "/assignment",
} as const;

export default path;
