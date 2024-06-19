import { useEffect, useState } from "react";
import { getStudyStudentDetail } from "@/apis/study.api";
import { useLocation, useNavigate } from "react-router-dom";
import Pagination from "@/components/pagination";
import { ColumnDef } from "@tanstack/react-table";
import { IClass } from "@/types/study.type";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";

export default function StudentDetailInfo() {
	const location = useLocation();
	const navigation = useNavigate();
	const state = location?.state;
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);
	const [student, setStudent] = useState<any>([]);
	const [semester] = useState<string>(`${state.academicYear.split(" ")[0]}1`);
	const columns: ColumnDef<IClass>[] = [
		{
			accessorKey: "subjectName",
			header: () => {
				return <div>Môn học</div>;
			},
			cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "weight1",
			header: () => {
				return <div>ĐĐGtx</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight1") as any[];
				return (
					<div>
						{weights.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
			minSize: 200,
		},
		{
			accessorKey: "weight2",
			header: () => {
				return <div>ĐĐGgk</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight2") as any[];
				return (
					<div>
						{weights.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
		},
		{
			accessorKey: "weight3",
			header: () => {
				return <div>ĐĐGck</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight3") as any[];
				return (
					<div>
						{weights.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
		},
		{
			accessorKey: "average",
			header: () => {
				return <div>ĐTBmhk</div>;
			},
			cell: ({ row }) => <div>{row.getValue("average")}</div>,
		},
	];

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);
	const handleGetData = () => {
		setLoading(true);
		getStudyStudentDetail(state.grade, semester, state.classDetailId).then(
			(res) => {
				setStudent(res.data?.data);
				setLoading(false);
			}
		);
	};

	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`KẾT QUẢ HỌC TẬP HỌC SINH ${state.fullName.toUpperCase()} `}</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={student}
					columns={columns}
					//onChange={handleChange}
					loading={loading}
				/>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					Chú Thích
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGtx: Điểm Đánh Giá thường xuyên
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGgk: Điểm Đánh Giá giữa kỳ
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGck: Điểm Đánh Giá cuối kỳ
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐTBmhk: Điểm Trung Bình môn học kỳ
				</div>
				<Pagination
					pageSize={pageSize}
					onChangePage={(value) => setPageNumber(Number(value))}
					onChangeRow={(value) => setPageSize(Number(value))}
					totalPageCount={totalPage}
				/>
			</div>
		</>
	);
}
