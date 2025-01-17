<template>
    <AppLayout>
        <section class="view-search">
            <SearchBar
                :is-searching="isSearching"
                :query="route.query?.query"
                @search="handleSearch"
            />
            <section
                class="search-loading"
                v-if="isSearching"
            >
                <SpinLoader />
            </section>
            <section
                class="search-results"
                v-if="!isSearching && searchResults !== null"
            >
                <ChartList
                    v-if="searchResultsType === 'charts'"
                    :charts="searchResults"
                />
                <UserList
                    v-if="searchResultsType === 'users'"
                    :users="searchResults"
                />
                <PlaylistList
                    v-if="searchResultsType === 'playlists'"
                    :playlists="searchResults"
                />
            </section>
            <SearchIntro v-if="!isSearching && searchResults === null" />
        </section>
    </AppLayout>
</template>

<script setup>
import { useRoute } from 'vue-router';
import AppLayout from '@/layouts/AppLayout.vue';
import { ref, onMounted, nextTick, inject } from 'vue';
import { searchCharts, searchUsers, searchPlaylists } from '@/api/api';
import SearchIntro from '@/components/Search/SearchIntro.vue';
import SearchBar from '@/components/Search/SearchBar.vue';
import ChartList from '@/components/Common/ChartList.vue';
import UserList from '@/components/Common/UserList.vue';
import PlaylistList from '@/components/Common/PlaylistList.vue';
import { Buttons, focusableElements } from '@/modules/useGamepad';
const emitter = inject('emitter');
import { useI18n } from 'vue-i18n';
const { t } = useI18n();

const isSearching = ref(false);
const searchResultsType = ref(null);
const searchResults = ref(null);

const route = useRoute();

const handleSearch = async (parameters) => {
    isSearching.value = true;

    searchResultsType.value = parameters.type;

    switch (parameters.type) {
        default:
        case 'charts':
            searchResults.value = await searchCharts(
                parameters.query,
                parameters.filters,
            );
            break;
        case 'playlists':
            searchResults.value = await searchPlaylists(parameters.query);
            break;
        case 'users':
            searchResults.value = await searchUsers(parameters.query);
            break;
    }

    isSearching.value = false;
};

onMounted(async () => {
    if (route.query.type && route.query.query) {
        handleSearch({
            type: route.query.type,
            query: route.query.query,
            filters: {
                showExplicit: true,
            },
        });
    }

    if (window.spinshare.settings.IsConsole) {
        // Select first Element
        await nextTick();
        const firstFocusableElement = document.body
            .querySelector('.view-search')
            .querySelector(focusableElements);

        if (firstFocusableElement) {
            firstFocusableElement.focus();
        }

        // Controller Hints
        let controllerHintItems = [];

        controllerHintItems.push({
            input: Buttons.A,
            label: t('general.select'),
            onclick: () => {
                const focussedElement = document.body.querySelector('*:focus');
                if (focussedElement) {
                    focussedElement.click();
                }
            },
        });

        emitter.emit('console-update-controller-hints', {
            showMenu: true,
            showBack: true,
            items: controllerHintItems,
        });
    }
});
</script>

<style lang="scss" scoped>
.view-search {
    display: grid;
    grid-template-rows: auto 1fr;
    height: 100%;
    overflow: hidden;

    & .search-loading {
        display: flex;
        justify-content: center;
        align-items: center;
    }
    & .search-results {
        overflow-y: scroll;
        padding: 40px;
    }
}
</style>
