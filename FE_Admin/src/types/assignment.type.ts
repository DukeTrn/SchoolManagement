export type IAssignmentDisplay = {
	assignmentId: string;
	teacherId: string;
	teacherName: string;
	email: string;
	phoneNumber: string;
	classId: string;
	className: string;
	semesterName: string;
	academicYear: string;
};

export type IAssignmentAdd = {
	semesterId: string;
	teacherId: string;
	subjectId: string;
	classId: string;
};

export type ISubject = {
	id: number;
	subjectName: string;
	description: string;
	grade: number;
};
