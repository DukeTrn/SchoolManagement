import path from "@/constants/path";
import MainLayout from "@/layouts/MainLayout";
import Home from "@/pages/home";
import { Navigate, useRoutes } from "react-router-dom";
import Account from "@/pages/account";
import Student from "@/pages/student";
import Department from "@/pages/department";
import DepartmentDetail from "@/pages/department/details/DepartmentDetail";
import Teacher from "@/pages/teacher/Teacher";

const useRouteElement = () => {
	const routeElement = useRoutes([
		{
			path: "/",
			element: <Navigate to={path.home} replace />,
		},
		{
			index: true,
			path: path.home,
			element: (
				<MainLayout>
					<Home />
				</MainLayout>
			),
		},
		{
			path: path.account,
			element: (
				<MainLayout>
					<Account />
				</MainLayout>
			),
		},
		{
			path: path.student,
			element: (
				<MainLayout>
					<Student />
				</MainLayout>
			),
		},
		{
			path: path.department,
			element: (
				<MainLayout>
					<Department />
				</MainLayout>
			),
		},
		{
			path: path.departmentDetails,
			element: (
				<MainLayout>
					<DepartmentDetail />
				</MainLayout>
			),
		},
		{
			path: path.teacher,
			element: (
				<MainLayout>
					<Teacher />
				</MainLayout>
			),
		},
	]);
	return routeElement;
};

export default useRouteElement;
