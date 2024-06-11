import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { useState } from "react";
import { v4 as uuidv4 } from "uuid";
import classNames from "classnames";

const RANGE = 1;
interface IPaginationProps {
	pageSize: number;
	onChangePage?: (value: string) => void;
	onChangeRow?: (value: string) => void;
}
const Pagination = (props: IPaginationProps) => {
	const { pageSize, onChangePage, onChangeRow } = props;
	const [currentPage, setCurrentPage] = useState<number>(1);

	const renderPagination = () => {
		let dotAfter = false;
		let dotBefore = false;

		//sau currentPage
		const onRenderDotAfter = () => {
			//se chi return ... 1 lan khi gap pageNumber thoa man
			if (!dotAfter) {
				dotAfter = true;
				return (
					<span
						key={uuidv4()}
						className="mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm"
					>
						...
					</span>
				);
			}
			return null;
		};

		//truoc currentPage
		const onRenderDotBefore = () => {
			if (!dotBefore) {
				dotBefore = true;
				return (
					<span
						key={uuidv4()}
						className="mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm"
					>
						...
					</span>
				);
			}
			return null;
		};
		return Array(pageSize)
			.fill(0)
			.map((_, index) => {
				const pageNumber = index + 1;
				if (
					currentPage <= RANGE * 2 + 1 &&
					pageNumber > currentPage + RANGE &&
					pageNumber <= pageSize - RANGE
				) {
					return onRenderDotAfter();
				} else if (
					currentPage > RANGE * 2 + 1 &&
					currentPage < pageSize - RANGE * 2
				) {
					if (
						pageNumber < currentPage - RANGE &&
						pageNumber > RANGE
					) {
						return onRenderDotBefore();
					} else if (
						pageNumber > currentPage + RANGE &&
						pageNumber <= pageSize - RANGE
					) {
						return onRenderDotAfter();
					}
				} else if (
					pageNumber < currentPage - RANGE &&
					pageNumber > RANGE &&
					currentPage >= pageSize - RANGE * 2
				) {
					return onRenderDotBefore();
				}
				return (
					<button
						key={uuidv4()}
						className={classNames(
							"mx-2 h-[40px] w-[40px] cursor-pointer rounded border bg-white/60 px-2 py-2 shadow-sm",
							{
								"bg-[#E11D47] text-white":
									pageNumber === currentPage,
								"": pageNumber !== currentPage,
							}
						)}
						onClick={() => {
							setCurrentPage(pageNumber);
							onChangePage && onChangePage(pageNumber.toString());
						}}
					>
						{pageNumber}
					</button>
				);
			});
	};
	return (
		<div>
			<div className="flex items-center justify-between space-x-2 py-4">
				<Select
					value={pageSize.toString()}
					onValueChange={(value) => {
						onChangeRow && onChangeRow(value);
					}}
				>
					<SelectTrigger className="h-10 w-[150px]">
						<SelectValue />
					</SelectTrigger>
					<SelectContent side="top">
						{[10, 20, 30, 40, 50].map((pageSize) => (
							<SelectItem key={pageSize} value={`${pageSize}`}>
								{pageSize}
							</SelectItem>
						))}
					</SelectContent>
				</Select>
				<div>
					{currentPage === 1 ? (
						<button className="mx-2 cursor-not-allowed rounded border bg-white/60 px-3 py-2 shadow-sm">
							Prev
						</button>
					) : (
						<button
							onClick={() => {
								onChangePage &&
									onChangePage((currentPage - 1).toString());
								setCurrentPage(
									(currentPage) => currentPage - 1
								);
							}}
							className="mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm "
						>
							Prev
						</button>
					)}

					{renderPagination()}

					{currentPage === pageSize ? (
						<button className="mx-2 cursor-not-allowed rounded border bg-white px-3 py-2 shadow-sm">
							Next
						</button>
					) : (
						<button
							onClick={() => {
								onChangePage &&
									onChangePage((currentPage + 1).toString());
								setCurrentPage(
									(currentPage) => currentPage + 1
								);
							}}
							className="mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm"
						>
							Next
						</button>
					)}
				</div>
			</div>
		</div>
	);
};

export default Pagination;
