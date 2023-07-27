import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { getAnimeFromId } from "@/lib/gql"
import { notFound } from "next/navigation"
import Image from "next/image";
import { AspectRatio } from "@/components/ui/aspect-ratio";
import { AnimeBar } from "@/components/AnimeBar";

export async function getRecommendedAnime(id: number) {
  const recommendationQuery = `query {
  Page {
    recommendations(mediaId: ${id}) {
      mediaRecommendation {
        id
        title {
          romaji
          english
        }
        startDate {
          year
          month
          day
        }
        coverImage {
          extraLarge
        }
        description
      }
    }
  }
}`;
  const res = await fetch('https://graphql.anilist.co', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      query: recommendationQuery
    }),
  });
  
  const data = await res
    .json()
    .then((res) =>
      res.data.Page.recommendations.map((rec: any) => rec.mediaRecommendation)
    );
  return data;
}

export default async function AnimePage({ params }: { params: { id: number } }) {
  let anime = (await getAnimeFromId([params.id]))
  const recommendations = await getRecommendedAnime(params.id)
  if (!anime) {
    notFound()
  }
  anime = anime[0]
  return (
    <div className="flex justify-center p-3 md:p-10">
      <Card className="w-full py-5 max-w-6xl">
        <CardHeader className="-md:text-center pb-2">
          <h1 className="text-2xl font-medium text-primary">
            {anime?.title?.english ?? anime?.title?.romaji}
          </h1>
          <p className="text-md font-light italic tracking-tighter">
            {anime?.title?.native}
          </p>
        </CardHeader>
        <div className="mx-5">
          <Separator className="mb-5"></Separator>
        </div>
        <CardContent className="">
          <div className="flex -md:flex-col -md:items-center gap-10">
            <div className="w-[250px] shrink-0">
              <AspectRatio ratio={3 / 4}>
                <Image
                  src={anime?.coverImage?.extraLarge ?? ''}
                  alt={anime?.title?.english ?? anime?.title?.romaji}
                  fill={true}
                  sizes="(max-width: 600px) 150px, 250px"
                  className="object-cover rounded-md"
                  priority={true}
                ></Image>
              </AspectRatio>
            </div>
            <Separator className="md:hidden"></Separator>
            <div className="md:h-[333px] overflow-y-auto md:mask-image py-1 md:pr-3">
              <h1 className="text-lg font-medium text-primary">Description</h1>
              <p className="mt-2 font-light">
                {anime?.description
                  ? anime.description.replace(/<[^>]+>/g, '')
                  : 'N/A'}
              </p>
            </div>
          </div>
          {recommendations.length !== 0 && (
            <>
              <Separator className="my-5"></Separator>
              <div className="space-y-5">
                <h1 className="text-lg font-medium text-primary">
                  Recommendations based on this anime:
                </h1>
                <AnimeBar
                  animeList={recommendations}
                  size="w-[180px]"
                ></AnimeBar>
              </div>
            </>
          )}
        </CardContent>
      </Card>
    </div>
  );
}