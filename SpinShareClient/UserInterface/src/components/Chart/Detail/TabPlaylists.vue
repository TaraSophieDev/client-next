<template>
    <section class="chart-detail-tab-playlists">
        <template v-if="playlists">
            <PlaylistList
                v-if="playlists.length > 0"
                :playlists="playlists"
            />
            <NoContent
                v-else
                :text="t('chart.detail.playlists.noContent')"
            />
        </template>
        <SpinLoader v-else />
    </section>
</template>

<script setup>
import { onMounted, ref } from 'vue';
import PlaylistList from '@/components/Common/PlaylistList.vue';
import { getChartPlaylists } from '@/api/api';
import NoContent from '../../Common/NoContent.vue';
import SpinLoader from '../../Common/SpinLoader.vue';
import { useI18n } from 'vue-i18n';
const { t } = useI18n();

const props = defineProps({
    id: {
        type: Number,
        default: 0,
    },
});

const playlists = ref(null);

onMounted(async () => {
    playlists.value = await getChartPlaylists(props.id);
});
</script>

<style lang="scss" scoped>
.chart-detail-tab-playlists {
    padding: 40px;
}
</style>
