const path = {
	login: "/login",
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
	study: "/study",
	studyClass: "/study/:id",
	studyStudent: "/study/:id/:id1",
	assignment: "/assignment",
	assignmentDetail: "assignment/:id",
} as const;

export default path;
