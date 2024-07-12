import { Input } from "@/components/ui/input";
import { Search } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { sideBarNavs } from "@/constants/nav";
import { v4 as uuidv4 } from "uuid";
import { IAppState, useAppSelector } from "@/redux/store";
import { useState } from "react";
import { useDebounce } from "@/hooks";
import { NavLink } from "react-router-dom";
import { cn } from "@/lib/utils";
import removeAccents from "remove-accents";

export default function Home() {
	const { role } = useAppSelector((state: IAppState) => state.users);
	const [search, setSearch] = useState("");
	const searchQuery = useDebounce(search, 500);

	const navs = sideBarNavs?.filter(
		(i) => i.role.includes(role!) || i.role.includes("All")
	);
	const navFilter = navs.filter((i) =>
		removeAccents(i.title.toLowerCase()).includes(
			removeAccents(searchQuery.toLowerCase())
		)
	);
	return (
		<div>
			<div className="text-3xl font-semibold uppercase">
				Chào mừng bạn đến với trang quản lý đào tạo
			</div>
			<div className="relative mt-10">
				<Search className="absolute left-2 top-2.5 h-4 w-4 " />
				<Input
					placeholder="Tìm kiếm"
					className="w-full pl-8"
					onChange={(e) => setSearch(e.target?.value)}
				/>
			</div>
			<div className="mb-5 mt-10">
				<div className="grid grid-cols-2 gap-x-10 gap-y-8 md:grid-cols-3 lg:grid-cols-4">
					{navFilter?.map((item) => {
						const isActive = location?.pathname.includes(
							item?.path
						);
						return (
							<NavLink
								key={item.title}
								to={`${item?.path}`}
								className={() =>
									cn(
										isActive
											? "justify-start text-sm text-white"
											: "justify-start text-sm text-black"
									)
								}
							>
								<Card
									key={uuidv4()}
									className={"flex flex-col items-center "}
								>
									<CardHeader>
										<CardTitle className="font-normal">
											<div
												className={`flex h-[60px] w-[60px] items-center justify-center rounded-sm bg-primary p-4`}
												style={{
													background:
														"linear-gradient(to left, hsl(346.8, 77.2%, 49.8%), hsl(346.8, 97.2%, 70%))",
												}}
											>
												<item.icon className="h-10 w-10 font-medium text-white" />
											</div>
										</CardTitle>
									</CardHeader>
									<CardContent>
										<div className="text-base font-medium">
											{item.title}
										</div>
									</CardContent>
								</Card>
							</NavLink>
						);
					})}
				</div>
			</div>
		</div>
	);
}
